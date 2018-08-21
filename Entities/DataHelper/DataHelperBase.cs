// using System;
// using System.Collections.Generic;
// using System.Data.Linq;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

// namespace CommonEntity.DataHelper
// {
//    public class DataHelperBase
//     {
//         public static void Update<T>(IEnumerable<T> objs, DataContext context, ConflictMode conflictMode= ConflictMode.FailOnFirstConflict) where T:class
//         {
//             if (objs == null || objs.Count() <= 0) return ;
//            var table= context.GetTable<T>();
//             table.AttachAll(objs);
//             context.Refresh(RefreshMode.KeepCurrentValues);
//             context.SubmitChanges(conflictMode);
//         }

//         public static void Insert<T>(IEnumerable<T> objs, DataContext context, ConflictMode conflictMode = ConflictMode.FailOnFirstConflict) where T : class
//         {
//             if (objs == null || objs.Count() <= 0) return;
//             var table = context.GetTable<T>();
//             table.InsertAllOnSubmit(objs);
//             context.SubmitChanges(conflictMode);
//         }
//         public static void Delete<T>(IEnumerable<T> objs, DataContext context, ConflictMode conflictMode = ConflictMode.FailOnFirstConflict) where T : class
//         {
//             if (objs == null || objs.Count() <= 0) return;
//             var table = context.GetTable<T>();
//             table.DeleteAllOnSubmit(objs);
//             context.SubmitChanges(conflictMode);
//         }
//     }
// }
