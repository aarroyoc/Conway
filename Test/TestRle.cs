using Xunit;
using Conway.File;

namespace Conway.Test {
    public class TestRle{
        [Fact]
        public void ReadHeader()
        {
            var rle = new Rle();
            rle.Load(@"Patterns/glider.rle");
        }
    }
}