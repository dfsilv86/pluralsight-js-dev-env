using System.Diagnostics.CodeAnalysis;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Classe base para entidades
    /// </summary>
    public abstract class EntityBase : IEntity
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="EntityBase"/>
        /// </summary>
        protected EntityBase()
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="EntityBase"/>.
        /// </summary>
        /// <param name="id">O id.</param>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected EntityBase(int id)
        {
            Id = id;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Obtém um valor que indica se é uma nova entidade ou se já existia.
        /// </summary>
        public bool IsNew
        {
            get
            {
                return Id == 0;
            }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="base1">The base1.</param>
        /// <param name="base2">The base2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(EntityBase base1, EntityBase base2)
        {
            // Check for both null (need this casts to object or will run in a recursive loop).
            if ((object)base1 == null && (object)base2 == null)
            {
                return true;
            }

            if ((object)base1 == null || (object)base2 == null)
            {
                return false;
            }
            
            return
                base1.Id.Equals(base2.Id)
             && base1.GetType() == base2.GetType();
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="base1">The base1.</param>
        /// <param name="base2">The base2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(EntityBase base1, EntityBase base2)
        {
            return !(base1 == base2);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as EntityBase;

            if (other == null)
            {
                return false;
            }

            return this == other;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion
    }
}