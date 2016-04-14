using System;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace SharpCqrs
{
    public static class SpecUtils
    {
        public static T TestValue<T>(this T value, string key = null)
        {
            if (key != null)
                ScenarioContext.Current.Set<T>(value, key);
            else
                ScenarioContext.Current.Set<T>(value);
            return value;
        }

        public static void TestValue<T>(this Func<T> action, string key = null)
        {
            T value = action();

            value.TestValue(key);
        }

        public static void TestValueProtected<T>(this Func<T> action, string key = null)
        {
            try
            {
                T value = action();

                value.TestValue(key);
            }
            catch (Exception ex)
            {
                ex.TestValue();
            }
        }

        public static void NoExceptionExpected()
        {
            Exception exception;
            if (ScenarioContext.Current.TryGetValue(out exception))
                Assert.Fail("No exception was expected, but the following was thrown. {0}: {1}", 
                    exception.GetType().FullName, exception.Message);
        }

        public static void ExceptionExpected<T>() where T: Exception
        {
            Exception exception;
            if (!ScenarioContext.Current.TryGetValue(out exception))
                Assert.Fail("An exception of type {0} was expected, but none was thrown.", 
                    typeof(T).FullName);
            else if (!(exception is T))
                Assert.Fail("An exception of type {0} was expected, but the following was thrown. {1}: {2}", 
                    typeof(T).FullName, exception.GetType().FullName, exception.Message);
        }
    }
}
