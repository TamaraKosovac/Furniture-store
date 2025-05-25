
namespace CozyHaven.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public string IconKind
        {
            get
            {
                return Name switch
                {
                    "Dnevna soba" => "SofaOutline",
                    "Spavaća soba" => "Bed",
                    "Kuhinja" => "Stove", 
                    "Kupatilo" => "Shower",
                    "Hodnik" => "DoorOpen",
                    "Radna soba" => "ChairRolling",
                    "Dječija soba" => "TeddyBear",
                    "Baštenski namještaj" => "PineTree", 
                    "Kancelarijski namještaj" => "Desk",
                    "Dekoracije" => "Lamps", 
                    _ => "Tag" 
                };
            }
        }
    }
}
