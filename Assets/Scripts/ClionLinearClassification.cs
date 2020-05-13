using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;


public class ClionLinearClassification : MonoBehaviour
{
    [DllImport("ML_5AL_lib_cpp_project_groupe_3")]
    private static extern IntPtr create_linear_model(int input_size);

    [DllImport("ML_5AL_lib_cpp_project_groupe_3")]
    private static extern int linear_model_train_classification(
        IntPtr model,
        double alpha,
        int epoch,
        double[] x_train,
        double[] y_train,
        int x_train_len,
        int y_train_len,
        int input_qty);

    [DllImport("ML_5AL_lib_cpp_project_groupe_3")]
    private static extern double linear_model_predict_classification(IntPtr model, double[] input, int input_qty);
    // Start is called before the first frame update

    void Start()
    {
        Debug.Log("started");
    }

    public Transform[] trainingSpheres;
    public Transform[] testSpheres;


    private double[] trainingInputs;
    private double[] trainingExpectedOutputs;

    private IntPtr model;

    public double alpha;
    
    public int epoch;

    public void ReInitialize()
    {
        for (var i = 0; i < testSpheres.Length; i++)
        {
            testSpheres[i].position = new Vector3(
                testSpheres[i].position.x,
                0f,
                testSpheres[i].position.z);
        }
    }
    
    public void CreateModel()
    {
        model = create_linear_model(2);
    }

    public void Train()
    {
        Debug.Log("Create Model");
        CreateModel();

        trainingInputs = new double[trainingSpheres.Length * 2];
        trainingExpectedOutputs = new double[trainingSpheres.Length];

        Debug.Log("Create input data set");
        for (var i = 0; i < trainingSpheres.Length; i++)
        {
            trainingInputs[2 * i] = trainingSpheres[i].position.x;
            trainingInputs[2 * i + 1] = trainingSpheres[i].position.z;
            trainingExpectedOutputs[i] = trainingSpheres[i].position.y;
        }

        Debug.Log("Start trainig");
        linear_model_train_classification(model, 0.1, 100,trainingInputs, trainingExpectedOutputs, trainingInputs.Length, trainingExpectedOutputs.Length, 2);
        Debug.Log("Trainig finished");

        
    }

    public void Predict()
    {
        Debug.Log("Predict Linear");
        if(model == IntPtr.Zero){
            Debug.Log("Model not found or not initialized");
            return;
        }
        
        Debug.Log($"Model ptr {model}");
        foreach (var t in testSpheres)
        {
            var input = new double[] {t.position.x, t.position.z};
            var predictedY = (float) linear_model_predict_classification(model, input, 2);
            t.position = new Vector3(
                t.position.x,
                predictedY,
                t.position.z);
        }
    }
}
