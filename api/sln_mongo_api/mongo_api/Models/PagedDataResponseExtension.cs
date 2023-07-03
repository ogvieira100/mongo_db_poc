//using Microsoft.EntityFrameworkCore.Query;
//using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace mongo_api.Models
{
    public static class PagedDataResponseExtension
    {


      public static async Task<PagedDataResponse<TModel>> PaginateAsync<TModel>(
      this IFindFluent<TModel, TModel> query,
      PagedDataRequest pagedDataRequest,
      Func<IFindFluent<TModel, TModel>> funcQueryOrder

      )
      where TModel : BaseMongo

        {

            var paged = new PagedDataResponse<TModel>();

            pagedDataRequest.Page = (pagedDataRequest.Page < 0) ? 1 : pagedDataRequest.Page;

            paged.Page = pagedDataRequest.Page;
            paged.PageSize = pagedDataRequest.Limit;

            long totalItemsCountTask = 0;

            
            totalItemsCountTask = await query.CountDocumentsAsync();
            

            if (funcQueryOrder is not null)
            {
                query = funcQueryOrder?.Invoke();

            }
            var startRow = (pagedDataRequest.Page - 1) * pagedDataRequest.Limit;

            if (startRow > 0)
                query =  query.Skip(startRow).Limit(paged.PageSize); ;


           
                paged.Items = await query
                           .ToListAsync();


            paged.TotalItens = totalItemsCountTask;
            paged.TotalPages = (int)Math.Ceiling(paged.TotalItens / (double)pagedDataRequest.Limit);

            return paged;


        }
        public static async Task<PagedDataResponse<TModel>> PaginateAsync<TModel>(
         this IMongoQueryable<TModel> query,
         PagedDataRequest pagedDataRequest,
         Func<IMongoQueryable<TModel>> funcQueryOrder

         )
         where TModel : BaseMongo

        {

            var paged = new PagedDataResponse<TModel>();

            pagedDataRequest.Page = (pagedDataRequest.Page < 0) ? 1 : pagedDataRequest.Page;

            paged.Page = pagedDataRequest.Page;
            paged.PageSize = pagedDataRequest.Limit;

            var totalItemsCountTask = 0;

            
            totalItemsCountTask = await query.CountAsync();
            

            if (funcQueryOrder is not null)
            {
                query = funcQueryOrder?.Invoke();

            }
            var startRow = (pagedDataRequest.Page - 1) * pagedDataRequest.Limit;

            if (startRow > 0)
                query = query.Skip(startRow);


            
            paged.Items = await query
                           .Take(pagedDataRequest.Limit)
                           .ToListAsync();

            
            paged.TotalItens = totalItemsCountTask;
            paged.TotalPages = (int)Math.Ceiling(paged.TotalItens / (double)pagedDataRequest.Limit);

            return paged;


        }
     //   public static async Task<PagedDataResponse<TModel>> PaginateAsync<TModel>(
     //    this IQueryable<TModel> query,
     //    PagedDataRequest pagedDataRequest,
     //    Func<IQueryable<TModel>> funcQueryOrder

     //)
     //where TModel : Base

     //   {

     //       var paged = new PagedDataResponse<TModel>();

     //       pagedDataRequest.Page = (pagedDataRequest.Page < 0) ? 1 : pagedDataRequest.Page;

     //       paged.Page = pagedDataRequest.Page;
     //       paged.PageSize = pagedDataRequest.Limit;

     //       var totalItemsCountTask = 0;

     //       if (query is IAsyncQueryProvider)
     //           totalItemsCountTask = await query.CountAsync();
     //       else
     //           totalItemsCountTask = query.Count();

     //       if (funcQueryOrder is null)
     //       {
     //           if (pagedDataRequest.Column.Length > 0 && pagedDataRequest.Desc)
     //               query = query.OrderByDescending(p => EF.Property<object>(p, pagedDataRequest.Column));
     //           else
     //               query = query.OrderBy(p => EF.Property<object>(p, pagedDataRequest.Column));

     //       }
     //       else
     //       {
     //           query = funcQueryOrder?.Invoke();
     //       }


     //       var startRow = (pagedDataRequest.Page - 1) * pagedDataRequest.Limit;

     //       if (startRow > 0)
     //           query = query.Skip(startRow);


     //       if (query is IAsyncQueryProvider)
     //           paged.Items = await query
     //                      .Take(pagedDataRequest.Limit)
     //                      .ToListAsync();

     //       else
     //           paged.Items = query
     //                 .Take(pagedDataRequest.Limit)
     //                 .ToList();

     //       paged.TotalItens = totalItemsCountTask;
     //       paged.TotalPages = (int)Math.Ceiling(paged.TotalItens / (double)pagedDataRequest.Limit);

     //       return paged;
     //   }
    }
}
