namespace API.Models.Base;

public interface IEntity {

    public long Id { get; }

    public DateTime CreationDate { get; }

    public DateTime? ChangeDate { get; set;}
    
}