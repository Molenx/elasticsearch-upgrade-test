namespace ElasticUpgradeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataGen = new DataGenerator();
            dataGen.WriteTomatosToElasticDirect();
        }
    }
}
