using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graphook
{
    internal class LinearRegressionModel
    {
        private float weightX;
        private float weightVelocity;
        private float weightHealth;
        private float bias;

        public LinearRegressionModel()
        {
            weightX = 0;
            weightVelocity = 0;
            weightHealth = 0;
            bias = 0;
        }

        // Train model using gradient descent
        public void Train(List<TrainingData> data, int epochs = 1000, float learningRate = 0.01f)
        {
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                float totalLoss = 0f;
                foreach (var sample in data)
                {
                    // Get predicted value for current sample
                    float prediction = Predict(sample.PlayerX, sample.PlayerVelocityX);

                    // Calculate error (loss)
                    float error = prediction - GetActionScore(sample.Action);
                    totalLoss += error * error;

                    // Update weights with gradient descent
                    weightX -= learningRate * error * sample.PlayerX;
                    weightVelocity -= learningRate * error * sample.PlayerVelocityX;
                    
                    bias -= learningRate * error;
                }

                if (epoch % 100 == 0)
                {
                    Console.WriteLine($"Epoch {epoch}: Loss = {totalLoss}");
                }
            }
        }

        // Predict the action score based on current player state
        public float Predict(float playerX, float velocity)
        {
            return weightX * playerX + weightVelocity * velocity + bias;
        }

        private float GetActionScore(string action)
        {
            switch (action)
            {
                case "MoveLeft": return 1;
                case "MoveRight": return 2;
                case "Jump": return 3;
                case "Attack": return 4;
                default: return 0; // Idle or any other default action
            }
        }
    }
}
