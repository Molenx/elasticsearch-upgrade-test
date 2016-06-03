namespace ElasticUpgradeTest
{
    using System.Runtime.Serialization;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    class TomatoShape
    {
        public enum ShapeType
        {
            [EnumMember(Value = "circle")]
            Circle,

            [EnumMember(Value = "polygon")]
            Polygon
        }

        #region

        public double[] Coordinates { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "type")]
        public ShapeType GeoType { get; set; }

        public decimal? Radius { get; set; }

        #endregion
    }
}