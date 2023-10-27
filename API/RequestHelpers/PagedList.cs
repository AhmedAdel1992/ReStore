using Microsoft.EntityFrameworkCore;

namespace API.RequestHelpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }

        public MetaData MetaData { get; set; }

        public static async Task<PagedList<T>> ToPagedList (IQueryable<T> query, int pageNumber, int pageSize)
        {
            var count = await query.CountAsync();
            //fo2 hna bn3ml el query el awal bel order bel search bel filters be kolo w b3d kda bngeb count el items elly tl3t mn el query da
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}

//el fekra mn elly fo2 da en el PagedList bta5od items w count w page number w page size w btrg3 el meta data bel items
//b3d kda ana 3mlt method ToPagedList zy el ToList kda 3shan ta5od el query elly h3mlo bel filters wel search w kolo w 
//b3d kda gowaha 7ddt el count b3d ma h3ml el query da w 7ddt ana 3ayz anhy page bel zabt 3n tre2 el skip wel take w b3d
//kda 3mlt return le PagedList edetha el items elly fel page elly 7dedtha bel skip wel take w edetha el count elly gbto
//bel countAsync mn el query w edetha el page number wel page size elly h7ddhom fel ToPagedList
//hst5dm dy b2a fel products controller
