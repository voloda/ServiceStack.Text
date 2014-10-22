using System;
using System.Diagnostics;
using NUnit.Framework;

namespace ServiceStack.Text.Tests
{
    [TestFixture]
    public class JsConfigDefaultCallbackTEsts
    {
        [Serializable]
        public class T1 {}
        [Serializable]
        public class T2 { }

        class TestCallback : IJsDefaultConfigCallback
        {
            public bool CalledForT1 { get; private set; }

            public void Callback<T>()
            {
                Debug.WriteLine(typeof(T));
                CalledForT1 = typeof(T) == typeof(T1);
                JsConfig<T>.RawSerializeFn = RawSerializeFn;
            }

            private string RawSerializeFn<T>(T arg)
            {
                throw new NotImplementedException();
            }
        }

        [TearDown]
        public void TearDown()
        {
            JsConfigDefaultCallback.Reset();
        }

        [Test]
        public void CallbackShouldBeCalledForT1AsExpected()
        {
            var callback = new TestCallback();
            JsConfigDefaultCallback.Register(callback);

            Assert.IsFalse(callback.CalledForT1);

            JsConfig<T1>.ExcludeTypeInfo = true;

            Assert.IsTrue(callback.CalledForT1);
        }
        
        [Test]
        public void CallbackShouldBeCalledForT2ButNotT1AsExpected()
        {
            var callback = new TestCallback();
            JsConfigDefaultCallback.Register(callback);

            Assert.IsFalse(callback.CalledForT1);

            JsConfig<T2>.ExcludeTypeInfo = true;

            Assert.IsFalse(callback.CalledForT1);
        }
    }
}