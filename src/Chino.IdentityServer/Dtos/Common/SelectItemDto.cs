namespace Chino.IdentityServer.Dtos.Common
{
    public class SelectItemDto
    {
        public SelectItemDto(string id, string text)
        {
            this.Id = id;
            this.Text = text;
        }

        public string Id { get; set; }
        public string Text { get; set; }
    }
}
