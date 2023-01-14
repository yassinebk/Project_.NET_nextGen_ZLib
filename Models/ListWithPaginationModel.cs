namespace Project.Models;

public class ListWithPaginationModel<TEntity>
{
    public ListWithPaginationModel(List<TEntity> listOfItemEntities, int totalNumber, int currentPage)
    {
        ListOfItemEntities = listOfItemEntities;
        TotalNumber = totalNumber;
        CurrentPage = currentPage;
    }

    public List<TEntity> ListOfItemEntities { set; get; }
    public int TotalNumber { set; get; }
    public int CurrentPage { set; get; }
}