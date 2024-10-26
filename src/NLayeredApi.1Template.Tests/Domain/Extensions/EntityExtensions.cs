using System.Reflection;

namespace NaviMente.Tests.Domain.Extensions
{
    internal static class EntityExtensions
    {
        /// <summary>
        /// Establece el valor de una propiedad de la entidad
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        //internal static T SetProperty<T>(this T entity, string propertyName, object value) where T : Entity
        //{
        //    if (entity is null)
        //        throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

        //    PropertyInfo? propertyInfo = entity.GetType().GetProperty(propertyName);
        //    propertyInfo?.SetValue(entity, value);

        //    return entity;
        //}

        /// <summary>
        /// Añade un elemento a una propiedad colección de la entidad
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Q"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        //internal static T AddElement<T, Q>(this T entity, string propertyName, Q value) where T : Entity where Q : Entity
        //{
        //    if (entity is null)
        //        throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

        //    PropertyInfo? propertyInfo = entity.GetType().GetProperty(propertyName);
        //    ICollection<Q>? collection = propertyInfo?.GetValue(entity, null) as ICollection<Q>;
        //    if (collection is null)
        //        collection = new List<Q>();
        //    collection.Add(value);
        //    propertyInfo?.SetValue(entity, collection);

        //    return entity;
        //}
    }
}
