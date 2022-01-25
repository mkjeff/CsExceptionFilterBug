using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CsExceptionFilterBug
{

    static class C
    {
        static async Task Main()
        {
            Debug.Assert(await ExceptionFilterBroken());
            Debug.Assert(await ExceptionFilterNormal1());
            Debug.Assert(await ExceptionFilterNormal1());
            Debug.Assert(await ExceptionFilterNormal1());
        }

        public static async Task<bool> ExceptionFilterNormal1()
        {
            try
            {
                await ThrowException();
                return true;
            }
            catch (Exception ex) when (ex.InnerException is ApplicationException error && (error.Message == "bad dog" || error.Message == "dog bad"))
            {
                return await FooAsync();
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> ExceptionFilterNormal2()
        {
            try
            {
                await ThrowException();
                return true;
            }
            catch (Exception ex) when (ex.InnerException is ApplicationException { Message: "bad dog" or "dog bad" })
            {
                return Foo();
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> ExceptionFilterNormal3()
        {
            try
            {
                await ThrowException();
                return true;
            }
            catch (Exception ex) when (ex.InnerException is ApplicationException { Message: "bad dog" })
            {
                return await FooAsync();
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> ExceptionFilterBroken()
        {
            try
            {
                await ThrowException();
                return true;
            }
            catch (Exception ex) when (ex.InnerException is ApplicationException { Message: "bad dog" or "dog bad" })
            {
                return await FooAsync();
            }
            catch
            {
                return false;
            }
        }

        static Task ThrowException() => throw new Exception("", new ApplicationException("bad dog"));
        static Task<bool> FooAsync() => Task.FromResult(true);
        static bool Foo() => true;
    }
}