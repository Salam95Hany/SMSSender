using SMSSender.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Services.Common
{
    public class GenericErrors
    {
        public static Error GetSuccess = new("اكتملت العملية بنجاح");

        public static Error AddSuccess = new("تمت الإضافة بنجاح");

        public static Error UpdateSuccess = new("تم التعديل بنجاح");

        public static Error DeleteSuccess = new("تم الحذف بنجاح");

        public static Error TransFailed = new("لقد حدث خطأ");

        public static Error NotFound = new("هذا العنصر غير موجود");

        public static Error InvalidCredentials = new("اسم المستخدم أو كلمة المرور غير صحيحة");

        public static Error DuplicateEmail = new("البريد الإلكتروني مسجل بالفعل");

        public static Error SuccessLogin = new("تم تسجيل الدخول بنجاح");

        public static Error SuccessRegister = new("تم تسجيل المستخدم بنجاح");

        public static Error AlreadyExists = new("هذا العنصر موجود");

        public static Error UserNotFound = new("لم يتم العثور على المستخدم");

        public static Error EmailAlreadyExists = new("البريد الإلكتروني مستخدم بالفعل من قبل مستخدم آخر");

        public static Error FailedToUpdateEmail = new("فشل تحديث البريد الإلكتروني");

        public static Error FailedToUpdatePassword = new("فشل تحديث كلمة المرور");

        public static Error FailedToAssignNewRole = new("فشل تعيين صلاحية جديدة");

        public static Error DeletePassFailed = new("فشل حذف كلمة المرور القديمة");

        public static Error NewPassFailed = new("كلمة مرور جديدة غير صالحة");

        public static Error UpdateRoleFailed = new("فشل تحديث صلاحية المستخدم");

        public static Error DelayedTransactionsExist = new("توجد معاملات متأخرة ضمن النطاق الزمني المحدد. يرجى حلها قبل حساب صافي الربح.");

        public static Error ProfitPeriodClosingSuccess = new("تم قفل الفترة بنجاح.");

        public static Error DeviceIsExist = new("معرف الجهاز او رقم الشريحة او اسم الشريحة موجود.");
    }
}
