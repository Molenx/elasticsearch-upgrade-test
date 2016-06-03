using System;

namespace ElasticUpgradeTest
{
    using System.Configuration;

    using Elasticsearch.Net;

    using Nest;

    class DataGenerator
    {

        private readonly Random randomGen;

        private readonly TomatoShape randomDataArea;

        protected readonly ElasticClient ElasticNestClient;

        protected readonly string EsIndexName;

        public DataGenerator()
        {
            // Elastic config
            var node = new Uri(ConfigurationManager.AppSettings["ElasticsearchNodeUrl"]);

            var connectionPool = new SniffingConnectionPool(new[] { node });
            var settings = new ConnectionSettings(connectionPool);

            ElasticNestClient = new ElasticClient(settings);
            EsIndexName = ConfigurationManager.AppSettings["ElasticsearchIndexName"];

            randomGen = new Random();
            randomDataArea = new TomatoShape { Radius = 200000, Coordinates = new[] { -84.552854, 42.737539 } };
        }

        public void WriteTomatosToElasticDirect()
        {
            Console.WriteLine("Started indexing tomatoes on {0}.", DateTime.Now);

            int tomatoCounter = 0;
            int batchSize = int.Parse(ConfigurationManager.AppSettings["BatchSize"]);


            var bulkDescriptor = new BulkDescriptor();

            for (int i = 0; i < batchSize; i++)
            {
                var tomato = GetRandomTomato(i);
                bulkDescriptor.Index<Tomato>(op => op.Document(tomato).Index(EsIndexName));

                tomatoCounter++;
            }

            var result = ElasticNestClient.Bulk(bulkDescriptor);

            Console.WriteLine("Indexed {0} tomatoes", tomatoCounter);
            Console.WriteLine("Tomatoes indexing completed on {0}, press ENTER to exit.", DateTime.Now);

            Console.Read();
        }

        private Tomato GetRandomTomato(int batchNo)
        {
            TomatoShape randomGeoShape = GetRandomGeoCircle();

            var tomato = new Tomato
            {
                Id = "tomato_" + Guid.NewGuid(),
                IsPublic = true,
                Name = $"Tomato  batchNo{batchNo}",
                Description = $"Random  batchNo{batchNo}",
                TomShape = randomGeoShape,
                FarmId = "farm_" + Guid.NewGuid(),
                DateTimeCreated = DateTime.Now
            };

            tomato.Name = $"Random name for tomato with id ={tomato.Id}";
            tomato.Description = $"Random description for tomato with id ={tomato.Id}";

            return tomato;
        }

        private TomatoShape GetRandomGeoCircle()
        {
            // http://gis.stackexchange.com/questions/25877/how-to-generate-random-locations-nearby-my-location
            double radiusInDegrees = double.Parse(randomDataArea.Radius.ToString()) / 111300.0;

            double x0 = randomDataArea.Coordinates[0];
            double y0 = randomDataArea.Coordinates[1];
            double u = randomGen.NextDouble();
            double v = randomGen.NextDouble();
            double w = radiusInDegrees * Math.Sqrt(u);
            double t = 2 * Math.PI * v;
            double x = w * Math.Cos(t);
            double y = w * Math.Sin(t);

            // Adjust the x-coordinate for the shrinking of the east-west distances
            double newX = x / Math.Cos(y0);

            var newGeoCircle = new TomatoShape { Radius = 4444, Coordinates = new[] { newX + x0, y + y0 } };

            return newGeoCircle;
        }
    }

}
