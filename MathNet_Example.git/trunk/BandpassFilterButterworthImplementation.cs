//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ButterWorth_Filter
//{
//    class BandpassFilterButterworthImplementation
//    {
//    }
//}
using System;

namespace DSP
{
    public class BandpassFilterButterworthImplementation
    {
        protected LowpassFilterButterworthImplementation lowpassFilter;
        protected HighpassFilterButterworthImplementation highpassFilter;

        public BandpassFilterButterworthImplementation(double bottomFrequencyHz, double topFrequencyHz, int numSections, double Fs)
        {
            this.lowpassFilter  = new LowpassFilterButterworthImplementation(topFrequencyHz, numSections, Fs);
            this.highpassFilter = new HighpassFilterButterworthImplementation(bottomFrequencyHz, numSections, Fs);
        }

        public double compute(double input)
        {
            // compute the result as the cascade of the highpass and lowpass filters
            return this.highpassFilter.compute(this.lowpassFilter.compute(input));
        }
    }
}