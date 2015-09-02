/*
' Copyright (c) 2015 Will Strohl
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using Newtonsoft.Json.Converters;

namespace WillStrohl.Modules.DNNHangout.Components
{
    public class DateTimeConverter : IsoDateTimeConverter
    {
        public DateTimeConverter()
        {
            base.DateTimeFormat = "MM/dd/yyyy hh:mm tt";
        }

        //public static DateTime ConvertToTimestamp(long value)
        //{
        //    //create Timespan by subtracting the value provided from
        //    //the Unix Epoch
        //    DateTime epoc = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        //    return epoc.AddSeconds((double)value);
        //}

        //public static DateTime ConvertToTimestamp(double value)
        //{
        //    var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        //    return origin.AddSeconds(value);
        //}

        //public long ToUnixTimespan(DateTime date)
        //{
        //    var tspan = date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        //    return (long)Math.Truncate(tspan.TotalSeconds);
        //}
    }
}