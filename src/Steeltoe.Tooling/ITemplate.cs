namespace Steeltoe.Tooling
{
    /// <summary>
    /// Template abstraction.
    /// </summary>
    public interface ITemplate
    {
        /// <summary>
        /// Binds the object to the named template variable.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        void Bind(string name, object obj);

        /// <summary>
        /// Returns the rendered template.
        /// </summary>
        /// <returns></returns>
        string Render();
    }
}
