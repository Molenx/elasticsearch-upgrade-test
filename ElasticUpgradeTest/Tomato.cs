namespace ElasticUpgradeTest
{
    using System;

    class Tomato
    {
        #region

        public string Id { get; set; }

        public string Description { get; set; }

        public TomatoShape TomShape { get; set; }

        public double[] TomatoCenter
        {
            get
            {
                if (this.TomShape?.Coordinates != null && this.TomShape.Coordinates.Length == 2)
                {
                    return this.TomShape.Coordinates;
                }
                return null;
            }
        }

        public bool? IsPublic { get; set; }

        public string Name { get; set; }

        public string Type => this.GetType().Name.ToLowerInvariant();

        public string FarmId { get; set; }

        public DateTime DateTimeCreated { get; set; }

        public DateTime? DateTimeModified { get; set; }

        #endregion
    }
}