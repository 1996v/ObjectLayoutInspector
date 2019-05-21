using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ObjectLayoutInspector.Tests
{
    [TestFixture]
    public class ME
    {
        [Test]
        public void Print_NotAlignedClass()
        {
            TypeLayout.PrintLayout<model>();
            model u = new model();
            u.FunC();
        }
    }

    public class model
    {
        int a;
        int b;
        Struct c;
        Class f = new Class();

        public struct Struct
        {
            public int d;
            public int e;
        }

        class Class
        {
            public int g;
            public int h;
        }

        public int FunA()
        {
            return 3;
        }

        public int FunB()
        {
            return b;
        }

        public void FunC()
        {
            /*
             |==============================|
                | Object Header (8 bytes)      |
                |------------------------------|
                | Method Table Ptr (8 bytes)   |
                |==============================|
                |   0-7: Class f (8 bytes)     |-->0:g,4:h
                |------------------------------|
                |  8-11: Int32 a (4 bytes)     |
                |------------------------------|
                | 12-15: Int32 b (4 bytes)     |
                |------------------------------|
                | 16-23: Struct c (8 bytes)    |
                | |==========================| |
                | |   0-3: Int32 d (4 bytes) | |
                | |--------------------------| |
                | |   4-7: Int32 e (4 bytes) | |
                | |==========================| |
                |==============================|
             
             */


            int num1 = c.e;//struct
            //00007FFAF04D1026 mov         rax,qword ptr[rbp + 60h]
            //00007FFAF04D102A mov         eax,dword ptr[rax + 1Ch] 1c=28 取28offset,28-8方法表=20 -->e的偏移量
            //00007FFAF04D102D mov         dword ptr[rbp + 34h], eax

            int num2 = f.h;//
            //00007FFAF04D1030 mov         rax,qword ptr[rbp + 60h]
            //00007FFAF04D1034 mov         rax,qword ptr[rax + 8]   取8offset,8-8方法表=0 -->f
            //00007FFAF04D1038 mov         eax,dword ptr[rax + 0Ch] 0c=12, 12-8=4, h的偏移量 
            //00007FFAF04D103B mov         dword ptr[rbp + 30h], eax

            int num3 = b;//
            //00007FFAF04D103E mov         rax,qword ptr[rbp + 60h]
            //00007FFAF04D1042 mov         eax,dword ptr[rax + 14h]
            //00007FFAF04D1045 mov         dword ptr[rbp + 2Ch], eax
        }
    }
}
