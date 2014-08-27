/*
' Copyright (c) 2014 Will Strohl
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

namespace WillStrohl.Modules.DNNHangout.Entities
{
    [Serializable]
    public class HangoutInfo : IHangoutInfo
    {

        public int ContentItemId { get; set; }

        public string HangoutAddress { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        
        public int Duration { get; set; }
        
        public DurationType DurationUnits { get; set; }

    }
}