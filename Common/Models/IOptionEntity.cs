namespace Common
{
    public interface IOptionEntity
    {
        int Orden { get; set; }
        string Nombre { get; set; }
        string Descripcion { get; set; }
        string Icono { get; set; }
        string Accion { get; set; }
        string Operaciones { get; set; }
        string Ayuda { get; set; }
    }
}