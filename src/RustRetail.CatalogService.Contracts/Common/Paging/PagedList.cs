namespace RustRetail.CatalogService.Contracts.Common.Paging
{
    /// <summary>
    /// Represents a paged list of items with pagination metadata.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    public class PagedList<T> where T : class
    {
        /// <summary>
        /// Gets the items on the current page.
        /// </summary>
        public List<T> Items { get; } = new List<T>();

        /// <summary>
        /// Gets the current page number (1-based).
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Gets the number of items per page.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether there is a next page.
        /// </summary>
        public bool HasNextPage => PageNumber * PageSize < TotalCount;

        /// <summary>
        /// Gets a value indicating whether there is a previous page.
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="items">The items on the current page.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="totalCount">The total number of items.</param>
        private PagedList(
            List<T> items,
            int pageNumber,
            int pageSize,
            int totalCount)
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        /// <summary>
        /// Creates a paged list from an <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <param name="query">The queryable source.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A new <see cref="PagedList{T}"/> instance.</returns>
        public static PagedList<T> Create(
            IQueryable<T> query,
            int pageNumber,
            int pageSize)
        {
            ValidatePageNavigations(pageNumber, pageSize);

            int totalCount = query.Count();
            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return new(items, pageNumber, pageSize, totalCount);
        }

        /// <summary>
        /// Creates a paged list from a list of items and total count.
        /// </summary>
        /// <param name="items">The items on the current page.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="totalCount">The total number of items.</param>
        /// <returns>A new <see cref="PagedList{T}"/> instance.</returns>
        public static PagedList<T> Create(
            List<T> items,
            int pageNumber,
            int pageSize,
            int totalCount,
            bool validate = true)
        {
            if (validate)
            {
                ValidatePageNavigations(pageNumber, pageSize);
                ValidateItemsAndTotalCount(items, totalCount);
            }

            return new(items, pageNumber, pageSize, totalCount);
        }

        /// <summary>
        /// Validates the page number and page size.
        /// </summary>
        /// <param name="pageNumber">The page number to validate.</param>
        /// <param name="pageSize">The page size to validate.</param>
        /// <exception cref="ArgumentException">Thrown if page number or page size is not greater than 0.</exception>
        private static void ValidatePageNavigations(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentException("Page number must be larger than 0.");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentException("Page size must be larger than 0.");
            }
        }

        /// <summary>
        /// Validates the items list and total count for consistency.
        /// </summary>
        /// <param name="items">The items list to validate.</param>
        /// <param name="totalCount">The total count to validate.</param>
        /// <exception cref="ArgumentException">Thrown if total count is negative or inconsistent with items.</exception>
        /// <exception cref="InvalidOperationException">Thrown if items and total count are inconsistent.</exception>
        private static void ValidateItemsAndTotalCount(List<T> items, int totalCount)
        {
            if (totalCount < 0)
            {
                throw new ArgumentException("Total item count must be equal or larger than 0.");
            }
            if (items != null)
            {
                if (items.Count > 0 && totalCount <= 0)
                {
                    throw new InvalidOperationException("Total item count must be larger than 0 when the items list contains element(s).");
                }
                if (items.Count == 0 && totalCount > 0)
                {
                    throw new InvalidOperationException("Total item count must be 0 when the items list contains no element.");
                }
            }
            else
            {
                if (totalCount > 0)
                {
                    throw new ArgumentException("Total item count must be 0 when the item list is null.");
                }
            }
        }
    }
}
