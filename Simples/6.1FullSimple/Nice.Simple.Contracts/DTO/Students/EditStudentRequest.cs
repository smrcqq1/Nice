namespace Nice.Simple.Contracts.DTO.Students
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class EditStudentRequest:AddStudentRequest
    {
        public Guid Id { get; set; }
    }
}
