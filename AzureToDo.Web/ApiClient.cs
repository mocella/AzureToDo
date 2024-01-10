using AzureToDo.Db.Entities;

namespace AzureToDo.Web;

public class ApiClient(HttpClient httpClient)
{
    public async Task<SupportTicket[]> GetSupportTicketsAsync()
    {
        return await httpClient.GetFromJsonAsync<SupportTicket[]>("/supportticket") ?? [];
    }
    public async Task PostSupportTicketsAsync(SupportTicket supportTicket)
    {
        await httpClient.PostAsJsonAsync<SupportTicket>("supportticket", supportTicket);
    }
}

