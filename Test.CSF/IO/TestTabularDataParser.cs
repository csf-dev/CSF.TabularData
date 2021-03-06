//
// TestTabularDataParser.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2015 CSF Software Limited
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using NUnit.Framework;
using CSF.TabularData;
using System.Collections.Generic;

namespace Test.CSF.IO
{
  [TestFixture]
  public class TestTabularDataParser
  {
    #region CSV tests
    
    [Test]
    public void TestReadCsv()
    {
      string input = "foo,bar,baz\r\n" +
                     "wibble,wobble,spong";
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.CreateCsv());
      
      var output = parser.Read(input);
      Assert.AreEqual(2, output.GetRowCount(), "Correct row count");
      Assert.AreEqual("wibble", output[1][0], "Correct data in second row, first column");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
    }
    
    [Test]
    public void TestReadCsvWithQuotes()
    {
      string input = "foo,bar,baz\r\n" +
                     "wibble,    wobble   ,spong\r\n" +
                     "\"  foo\",\"\"\"bar\"\"\",\"A big, \"\"big, test!\"";
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.CreateCsv());
      
      var output = parser.Read(input);
      Assert.AreEqual(3, output.GetRowCount(), "Correct row count");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
      Assert.AreEqual("  foo", output[2][0], "Correct data in third row, first column");
      Assert.AreEqual("\"bar\"", output[2][1], "Correct data in third row, second column");
      Assert.AreEqual("A big, \"big, test!", output[2][2], "Correct data in third row, third column");
    }
    
    [Test]
    public void TestReadCsvWithQuotedUnicode()
    {
      string input = "foo,bar,baz\r\n" +
                     "wibble,    wobble   ,spong\r\n" +
                     "\"  foo\",\"\"\"bar\"\"\",\"A big, \"\"big, ¥en test!\"";
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.CreateCsv());
      
      var output = parser.Read(input);
      Assert.AreEqual(3, output.GetRowCount(), "Correct row count");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
      Assert.AreEqual("  foo", output[2][0], "Correct data in third row, first column");
      Assert.AreEqual("\"bar\"", output[2][1], "Correct data in third row, second column");
      Assert.AreEqual("A big, \"big, ¥en test!", output[2][2], "Correct data in third row, third column");
    }
    
    [Test]
    public void TestWriteCsv()
    {
      string output;
      string expectedOutput = "foo,bar,baz\r\n" +
                              "wibble,wobble,spong";
      IList<IList<string>> input = new string[][] { new string[] { "foo",    "bar",    "baz"   },
                                                    new string[] { "wibble", "wobble", "spong" } };
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.CreateCsv());
      
      output = parser.Write(input);
      Assert.AreEqual(expectedOutput, output, "Correct string rendering");
    }
    
    [Test]
    public void TestWriteCsvWithQuotes()
    {
      string output;
      string expectedOutput = "foo,bar,baz\r\n" +
                              "wibble,wobble,spong\r\n" +
                              "\"  foo\",\"\"\"bar\"\"\",\"baz,bork\"";
      IList<IList<string>> input = new string[][] { new string[] { "foo",    "bar",     "baz"      },
                                                    new string[] { "wibble", "wobble",  "spong"    },
                                                    new string[] { "  foo",  "\"bar\"", "baz,bork" }};
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.CreateCsv());
      
      output = parser.Write(input);
      Assert.AreEqual(expectedOutput, output, "Correct string rendering");
    }
    
    [Test]
    [ExpectedException(typeof(TabularDataReadException),
                       ExpectedMessage = "Invalid tabular data; column count does not match first column at row 4.")]
    public void TestReadCsvInvalid()
    {
      string input = "r1c1,,\r\n" +
                     ",,\r\n" +
                     "r3c1,,\r\n";
      new TabularDataParser(TabularDataFormat.CreateCsv()).Read(input);
    }
    
    [Test]
    public void TestReadCsvEmptyLines()
    {
      string input = "r1c1,,\r\n" +
                     ",,\r\n" +
                     "r3c1,,";
      var output = new TabularDataParser(TabularDataFormat.CreateCsv()).Read(input);
      
      Assert.AreEqual("r1c1", output[0][0], "Row 1 column 1");
      Assert.AreEqual(String.Empty, output[0][2], "Row 1 column 3");
      Assert.AreEqual("r3c1", output[2][0], "Row 3 column 1");
    }

    [Test]
    public void TestReadCsvTolerateEmptyLines()
    {
      // Arrange
      string input = "r1c1,,\r\n" +
        "\r\n" +
        "r3c1,,";

      // Act
      var format = TabularDataFormat.CreateCsv(tolerateEmptyRows: true);
      var output = new TabularDataParser(format).Read(input);

      // Assert
      Assert.AreEqual(2, output.GetRowCount(), "Row count");
      Assert.AreEqual(3, output.GetColumnCount(), "Column count");
      Assert.AreEqual("r1c1", output[0][0], "Row 1 column 1");
      Assert.AreEqual("r3c1", output[1][0], "Row 2 column 1");
    }

    [Test]
    public void TestReadCsvTolerateTrailingLines()
    {
      // Arrange
      string input = "r1c1,,\r\n" +
        "r3c1,,\r\n" +
        "\r\n";

      // Act
      var format = TabularDataFormat.CreateCsv(tolerateEmptyRows: true);
      var output = new TabularDataParser(format).Read(input);

      // Assert
      Assert.AreEqual(2, output.GetRowCount(), "Row count");
      Assert.AreEqual(3, output.GetColumnCount(), "Column count");
      Assert.AreEqual("r1c1", output[0][0], "Row 1 column 1");
      Assert.AreEqual("r3c1", output[1][0], "Row 2 column 1");
    }

    #endregion
    
    #region TSV tests
    
    [Test]
    public void TestReadTsv()
    {
      string input = "foo\tbar\tbaz\n" +
                     "wibble\twobble\tspong";
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.CreateTsv());
      
      var output = parser.Read(input);
      Assert.AreEqual(2, output.GetRowCount(), "Correct row count");
      Assert.AreEqual("wibble", output[1][0], "Correct data in second row, first column");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
    }
    
    [Test]
    public void TestReadTsvWithQuotes()
    {
      string input = "foo\tbar\tbaz\n" +
                     "wibble\t    wobble   \tspong\n" +
                     "\"  foo\"\t\"\"\"bar\"\"\"\t\"A big, \"\"big, test!\"";
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.CreateTsv());
      
      var output = parser.Read(input);
      Assert.AreEqual(3, output.GetRowCount(), "Correct row count");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
      Assert.AreEqual("\"  foo\"", output[2][0], "Correct data in third row, first column");
      Assert.AreEqual("\"\"\"bar\"\"\"", output[2][1], "Correct data in third row, second column");
      Assert.AreEqual("\"A big, \"\"big, test!\"", output[2][2], "Correct data in third row, third column");
    }
    
    [Test]
    public void TestReadTsvWithQuotedUnicode()
    {
      string input = "foo\tbar\tbaz\n" +
                     "wibble\t    wobble   \tspong\n" +
                     "\"  foo\"\t\"\"\"bar\"\"\"\t\"A big, \"\"big, ¥en test!\"";
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.CreateTsv());
      
      var output = parser.Read(input);
      Assert.AreEqual(3, output.GetRowCount(), "Correct row count");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
      Assert.AreEqual("\"  foo\"", output[2][0], "Correct data in third row, first column");
      Assert.AreEqual("\"\"\"bar\"\"\"", output[2][1], "Correct data in third row, second column");
      Assert.AreEqual("\"A big, \"\"big, ¥en test!\"", output[2][2], "Correct data in third row, third column");
    }
    
    [Test]
    public void TestWriteTsv()
    {
      string output;
      string expectedOutput = "foo\tbar\tbaz\n" +
                              "wibble\twobble\tspong";
      IList<IList<string>> input = new string[][] { new string[] { "foo",    "bar",    "baz"   },
                                                    new string[] { "wibble", "wobble", "spong" } };
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.CreateTsv());
      
      output = parser.Write(input);
      Assert.AreEqual(expectedOutput, output, "Correct string rendering");
    }
    
    [Test]
    public void TestWriteTsvWithQuotes()
    {
      string output;
      string expectedOutput = "foo\tbar\tbaz\n" +
                              "wibble\t  wobble  \tspong\n" +
                              "  foo\t\"bar\"\tbaz,bork";
      IList<IList<string>> input = new string[][] { new string[] { "foo",    "bar",     "baz"      },
                                                    new string[] { "wibble", "  wobble  ",  "spong"    },
                                                    new string[] { "  foo",  "\"bar\"", "baz,bork" }};
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.CreateTsv());
      
      output = parser.Write(input);
      Assert.AreEqual(expectedOutput, output, "Correct string rendering");
    }
    
    #endregion

    #region writing from 2d arrays
    
    [Test]
    public void TestWriteCsvFrom2DArray()
    {
      string output;
      string expectedOutput = "foo,bar,baz\r\n" +
                              "wibble,wobble,spong";
      string[,] input = new string[,] { { "foo",    "bar",    "baz"   },
                                        { "wibble", "wobble", "spong" } };
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.CreateCsv());
      
      output = parser.Write(input);
      Assert.AreEqual(expectedOutput, output, "Correct string rendering");
    }
    
    [Test]
    public void TestWriteTsvFrom2DArray()
    {
      string output;
      string expectedOutput = "foo\tbar\tbaz\n" +
                              "wibble\twobble\tspong";
      string[,] input = new string[,] { { "foo",    "bar",    "baz"   },
                                        { "wibble", "wobble", "spong" } };
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.CreateTsv());
      
      output = parser.Write(input);
      Assert.AreEqual(expectedOutput, output, "Correct string rendering");
    }

    #endregion
  }
}

