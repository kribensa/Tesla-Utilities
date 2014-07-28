// ReSharper disable ConvertToConstant.Local
namespace TeslaUtilities.Music.Tests
{
    using System.IO;

    using NUnit.Framework;

    using Shouldly;

    [TestFixture]
    public class MusicTests
    {
        [TestFixture]
        public class FileRenamerTests
        {
            [TestCase]
            public void RenameNewFile()
            {
                var newFile = new FileInfo(@"c:\test\SomeNewFile.mp3");
                int newIndex = 5;
                int digitCount = 3;

                var renamer = new FileRenamer();
                var newName = renamer.GetNewFileName(newFile, newIndex, digitCount);

                newName.ShouldBe("005-SomeNewFile.mp3");
            }

            [TestCase]
            public void RenameExistingFileSameDigitCount()
            {
                var newFile = new FileInfo(@"c:\test\002-SomeNewFile.mp3");
                int newIndex = 5;
                int digitCount = 3;

                var renamer = new FileRenamer();
                var newName = renamer.GetNewFileName(newFile, newIndex, digitCount);

                newName.ShouldBe("005-SomeNewFile.mp3");
            }

            [TestCase]
            public void RenameExistingFileDifferentDigitCount()
            {
                var newFile = new FileInfo(@"c:\test\02-SomeNewFile.mp3");
                int newIndex = 5;
                int digitCount = 3;

                var renamer = new FileRenamer();
                var newName = renamer.GetNewFileName(newFile, newIndex, digitCount);

                newName.ShouldBe("005-SomeNewFile.mp3");
            }

            [TestCase]
            public void RenameNewFileWithNumericPrefix()
            {
                var newFile = new FileInfo(@"c:\test\2SomeNewFile.mp3");
                int newIndex = 5;
                int digitCount = 3;

                var renamer = new FileRenamer();
                var newName = renamer.GetNewFileName(newFile, newIndex, digitCount);

                newName.ShouldBe("005-2SomeNewFile.mp3");
            }

        }
    }
}
// ReSharper restore ConvertToConstant.Local