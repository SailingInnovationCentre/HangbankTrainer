using System;
using System.ComponentModel;

namespace HangbankTrainer.Model
{
    public class TrainingStatus
    {
        private TrainingTypeEnum TrainingType;

        private readonly bool _isTrainingActive;
        private readonly bool _isOutOfTraining;
        private readonly int _intervalNr;
        private readonly int _remainingSeconds;

        public TrainingStatus(double t, Training training)
        {
            // Please refer to the 'picture' at the top of the Training class to understand the interval numbers etc. 

            TrainingType = training.TrainingType;

            if (training.TrainingType == TrainingTypeEnum.Constant)
            {
                _intervalNr = 0;
                _remainingSeconds = (int) t;
                _isTrainingActive = t < 0;
                _isOutOfTraining = t < 0;
            }
            else if (training.TrainingType == TrainingTypeEnum.Interval)
            {
                if (t < 0 || t > training.NrOfIntervals * (training.SecondsRest + training.SecondsTraining))
                {
                    // Out of training period. 
                    _isOutOfTraining = true; 
                    _intervalNr = 0;
                    _remainingSeconds = 0;
                    _isTrainingActive = false;
                } 
                else
                {
                    _isOutOfTraining = false;

                    int elapsedSeconds = (int)(Math.Floor(t-1.0));  // Drawing runs 1 second behind, so it should be compensated. 
                    _intervalNr = (elapsedSeconds / (training.SecondsTraining + training.SecondsRest)) + 1;
                    int secondsWithinInterval = elapsedSeconds % (training.SecondsTraining + training.SecondsRest);

                    if (secondsWithinInterval < training.SecondsTraining)
                    {
                        _isTrainingActive = true;
                        _remainingSeconds = training.SecondsTraining - secondsWithinInterval ;
                    }
                    else
                    {
                        _isTrainingActive = false;
                        _remainingSeconds = training.SecondsRest - (secondsWithinInterval - training.SecondsTraining);
                    }
                }
            }
            else
            {
                throw new InvalidEnumArgumentException($"Unknown training type: {training.TrainingType}.");
            }
        }

        public string GetStatusText()
        {
            if (_isOutOfTraining)
            {
                return "---"; 
            }

            if (TrainingType == TrainingTypeEnum.Constant)
            {
                TimeSpan ts = TimeSpan.FromSeconds(_remainingSeconds);
                return ts.ToString(@"mm\:ss");
            }
            else if (TrainingType == TrainingTypeEnum.Interval)
            {
                TimeSpan ts = TimeSpan.FromSeconds(_remainingSeconds);
                return $"Interval {_intervalNr}, " + ts.ToString(@"mm\:ss");
            }
            else
            {
                throw new InvalidEnumArgumentException($"Unknown training type: {TrainingType}.");
            }
        }
    }
}
