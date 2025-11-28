public class Passenger
{
    public int PassengerId { get; set; }
    public string FullName { get; set; }
    public string PassportNumber { get; set; }
    public string VisaType { get; set; }
    public string Nationality { get; set; }
    public DateOnly ArrivalDate { get; set; }
    public int ArrivalYear { get; set; }
    public string PurposeOfVisit { get; set; }
    public int OfficerId { get; set; }
}
