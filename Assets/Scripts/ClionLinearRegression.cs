using UnityEngine;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;
using System;

public class ClionLinearRegression: MonoBehaviour
{
    [DllImport("ML_5AL_lib_cpp_project_groupe_3")]
    private static extern void hello();

    [DllImport("ML_5AL_lib_cpp_project_groupe_3")]
    private static extern IntPtr create_linear_model(int input_size);

    [DllImport("ML_5AL_lib_cpp_project_groupe_3")]
    private static extern int linear_model_train_regression(
        IntPtr model,
        double[] x_train,
        double[] y_train,
        int x_train_len,
        int y_train_len,
        int input_size);

    [DllImport("libML_5AL_lib_cpp_project_groupe_3")]
    private static extern double linear_model_predict_regression(IntPtr model, double[] input, int input_size);


    public Transform[] trainSpheresTransforms;
    public Transform[] testSpheresTransforms;

    
    private double[] trainingInputs;
    private double[] trainingExpectedOutputs;

    private IntPtr model;

    public void CreateModel()
    {
        model = create_linear_model(2);
    }

    public void Hello(){
        hello();
        Debug.Log("Hello Hello !!");
    }

    void Start()
    {
        Hello();
    }

    public void Train()
    {
        Debug.Log("Create Model");
        CreateModel();

        trainingInputs = new double[trainSpheresTransforms.Length * 2];
        trainingExpectedOutputs = new double[trainSpheresTransforms.Length];

        // Créer dataset_inputs
        Debug.Log("Create input data set");

        for (var i = 0; i < trainSpheresTransforms.Length; i++)
        {
            trainingInputs[2 * i] = trainSpheresTransforms[i].position.x;
            trainingInputs[2 * i + 1] = trainSpheresTransforms[i].position.z;
            trainingExpectedOutputs[i] = trainSpheresTransforms[i].position.y;
        }

        linear_model_train_regression(model, trainingInputs, trainingExpectedOutputs, trainingInputs.Length, trainingExpectedOutputs.Length, 2);

        // Créer dataest_expected_outputs

        // Create Model

        // Train Model

        // For each testSphere : Predict 

        // Delete Model
    }

    public void Predict()
    {
        Debug.Log("Predict Linear");
        if(model == IntPtr.Zero){
            Debug.Log("Model not found or not initialized");
            return;
        }
        foreach (var sphere in testSpheresTransforms)
        {
            var input = new double[] {sphere.position.x, sphere.position.z};
            //var predictedY = PredictXXXLinearModel(model, input, 2)
            Debug.Log($"inputs : {input}");
            //Debug.Log($"predict : {linear_model_predict_regression(model, input, 2)}");
            var predictedY = (float) linear_model_predict_regression(model, input, 2);
            sphere.position = new Vector3(
                sphere.position.x,
                predictedY,
                sphere.position.z);
        }
    }

}
