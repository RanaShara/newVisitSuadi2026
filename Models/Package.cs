namespace newVisitSuadi2026.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int CityId { get; set; }
        public int TourCompanyId { get; set; }
      public string Details { get; set; }
        public string ImagePath{ get; set; }
        public int TourGuideId { get; set; }


    }
}

