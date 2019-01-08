using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

/// <summary>
/// Representa uma AlcadaDetalhe.
/// </summary>
public class AlcadaDetalhe : EntityBase, IAggregateRoot
{
    /// <summary>
    /// Obtém ou define Id.
    /// </summary>
    public override int Id
    {
        get
        {
            return this.IDAlcadaDetalhe; 
        }

        set
        {
            this.IDAlcadaDetalhe = value;
        }
    }

    /// <summary>
    /// Obtém ou define idAlcadaDetalhe.
    /// </summary>
    public int IDAlcadaDetalhe { get; set; }

    /// <summary>
    /// Obtém ou define IDAlcada.
    /// </summary>
    public int IDAlcada { get; set; }

    /// <summary>
    /// Obtém ou define IDRegiaoAdministrativa.
    /// </summary>
    public int IDRegiaoAdministrativa { get; set; }

    /// <summary>
    /// Obtém ou define IDBandeira.
    /// </summary>
    public int IDBandeira { get; set; }

    /// <summary>
    /// Obtém ou define IDDepartamento.
    /// </summary>
    public int IDDepartamento { get; set; }

    /// <summary>
    /// Obtém ou define vlPercentualAlterado.
    /// </summary>
    public decimal vlPercentualAlterado { get; set; }

    /// <summary>
    /// Obtém ou define RegiaoAdministrativa.
    /// </summary>
    public RegiaoAdministrativa RegiaoAdministrativa { get; set; }

    /// <summary>
    /// Obtém ou define Bandeira.
    /// </summary>
    public Bandeira Bandeira { get; set; }

    /// <summary>
    /// Obtém ou define Departamento.
    /// </summary>
    public Departamento Departamento { get; set; }
}