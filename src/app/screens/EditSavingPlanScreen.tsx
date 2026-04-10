import { ChevronLeft } from 'lucide-react';
import { useNavigate } from 'react-router';
import StatusBar from '../components/StatusBar';

export default function EditSavingPlanScreen() {
  const navigate = useNavigate();
  const daysInMonth = Array.from({ length: 35 }, (_, i) => i + 1);

  return (
    <div className="min-h-screen bg-[#0A1428]">
      <div className="bg-gradient-to-b from-[#0A1428] to-[#0335a3] pb-6">
        <StatusBar />

        <div className="px-6 pt-4 flex items-center gap-4">
          <button onClick={() => navigate(-1)} className="text-white">
            <ChevronLeft className="w-6 h-6" />
          </button>
          <h1 className="text-white text-xl font-semibold">Edit Saving Plan</h1>
        </div>
      </div>

      <div className="px-6 pt-6 space-y-6">
        <div>
          <label className="text-white text-sm mb-2 block">Plan Name</label>
          <input
            type="text"
            defaultValue="Gaming PC"
            className="w-full bg-[#3E4E65] text-white px-4 py-3 rounded-xl outline-none focus:ring-2 focus:ring-[#00B2FF]"
          />
        </div>

        <div>
          <label className="text-white text-sm mb-2 block">Target Amount</label>
          <div className="relative">
            <span className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400">₱</span>
            <input
              type="number"
              defaultValue="40,000"
              className="w-full bg-[#3E4E65] text-white px-10 py-3 rounded-xl outline-none focus:ring-2 focus:ring-[#00B2FF]"
            />
          </div>
        </div>

        <div>
          <label className="text-white text-sm mb-2 block">Select Date</label>
          <div className="bg-[#3E4E65] rounded-2xl p-4">
            <div className="flex justify-around mb-4">
              <button className="text-[#00B2FF] font-semibold">Start Date</button>
              <button className="text-gray-400 font-semibold">Added</button>
              <button className="text-gray-400 font-semibold">Recurring</button>
              <button className="text-gray-400 font-semibold">Not Available</button>
            </div>

            <div className="grid grid-cols-7 gap-2 mb-4">
              {['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'].map((day) => (
                <div key={day} className="text-center text-gray-400 text-xs font-semibold">
                  {day}
                </div>
              ))}
            </div>

            <div className="grid grid-cols-7 gap-2">
              {daysInMonth.slice(0, 35).map((day) => (
                <button
                  key={day}
                  className={`aspect-square rounded-full flex items-center justify-center text-sm ${
                    day === 10
                      ? 'bg-[#00B2FF] text-white font-semibold'
                      : day > 31
                      ? 'text-gray-600'
                      : 'text-gray-300 hover:bg-[#2A3A52]'
                  }`}
                >
                  {day <= 31 ? day : day - 31}
                </button>
              ))}
            </div>
          </div>
        </div>

        <div>
          <label className="text-white text-sm mb-2 block">Duration</label>
          <select className="w-full bg-[#3E4E65] text-white px-4 py-3 rounded-xl outline-none focus:ring-2 focus:ring-[#00B2FF]">
            <option>1 Month</option>
            <option>3 Months</option>
            <option>6 Months</option>
            <option>1 Year</option>
          </select>
        </div>

        <button className="w-full bg-[#00B2FF] text-white py-4 rounded-full font-semibold hover:bg-[#00A8E8] transition-colors">
          Save Plan
        </button>
      </div>
    </div>
  );
}
