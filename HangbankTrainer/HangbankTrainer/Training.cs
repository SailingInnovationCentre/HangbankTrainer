namespace HangbankTrainer
{

    public enum TrainingTargetType
    {
        ConstantLow, 
        ConstantMid,
        ConstantHigh 
    }

    public class Training
    {
        public double Bandwidth { get; set; }

        public TrainingTargetType Scheme { get; set; }

        public Training()
        {
            Bandwidth = 2.0; 
        }
    }
}