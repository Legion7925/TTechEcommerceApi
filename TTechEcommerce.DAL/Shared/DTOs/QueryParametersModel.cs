namespace TTechEcommerceApi.Model
{
    public class QueryParametersModel
    {
        /// <summary>
        /// provide a max size so user can not query a lot of data
        /// </summary>
        const int _maxSize = 100;
        private int _size = 50;

        public int Page { get; set; } = 1;

        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = Math.Min(value, _maxSize);
            }
        }
    }
}
