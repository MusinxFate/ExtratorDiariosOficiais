namespace ExtratorDiariosOficiais;

public class JsonObject
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class JsonArray
    {
        public string pubName { get; set; }
        public string urlTitle { get; set; }
        public string numberPage { get; set; }
        public string subTitulo { get; set; }
        public string titulo { get; set; }
        public string title { get; set; }
        public string pubDate { get; set; }
        public string content { get; set; }
        public string editionNumber { get; set; }
        public int hierarchyLevelSize { get; set; }
        public string artType { get; set; }
        public string pubOrder { get; set; }
        public string hierarchyStr { get; set; }
        public List<string> hierarchyList { get; set; }
    }

    public class Root
    {
        public TypeNormDay typeNormDay { get; set; }
        public string idPortletInstance { get; set; }
        public string dateUrl { get; set; }
        public string section { get; set; }
        public List<JsonArray> jsonArray { get; set; }
    }

    public class TypeNormDay
    {
        public bool DO2ESP { get; set; }
        public bool DO1ESP { get; set; }
        public bool DO1A { get; set; }
        public bool DO3E { get; set; }
        public bool DO2E { get; set; }
        public bool DO1E { get; set; }
    }
}