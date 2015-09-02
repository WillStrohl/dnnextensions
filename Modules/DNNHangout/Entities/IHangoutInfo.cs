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

namespace WillStrohl.Modules.DNNHangout.Entities
{
    public interface IHangoutInfo
    {

        int ContentItemId { get; set; }

        string HangoutAddress { get; set; }
        
        string Title { get; set; }

        string Description { get; set; }

        DateTime StartDate { get; set; }

        int Duration { get; set; }

        DurationType DurationUnits { get; set; }

    }
}