//
// IParameterParser.cs
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
using System.Collections.Generic;

namespace CSF.Cli.Parameters
{
  /// <summary>
  /// Base interface for a parameter parser.
  /// </summary>
  public interface IParameterParser
  {
    #region methods

    /// <summary>
    /// Gets a read-only collection of the parameters registered for the current instance.
    /// </summary>
    /// <returns>A read-only collection of the registered parameters.</returns>
    ParameterMapping[] GetRegisteredParameters();

    /// <summary>
    /// Parses the given command line arguments into a <see cref="ParsedParameters"/> instance.
    /// </summary>
    /// <param name="commandlineArguments">The command line arguments.</param>
    ParsedParameters Parse(IList<string> commandlineArguments);

    #endregion
  }
}

