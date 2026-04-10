import { ChevronLeft } from 'lucide-react';
import { useNavigate } from 'react-router';
import StatusBar from '../components/StatusBar';
import imgProfile from 'figma:asset/69ca7ab92d67aa29944f86cbdd255cc6ff75a5f7.png';

export default function AddTransactionScreen() {
  const navigate = useNavigate();

  const categories = [
    { icon: '🍔', label: 'Food' },
    { icon: '🚗', label: 'Traffic' },
    { icon: '🛍️', label: 'Shopping' },
    { icon: '🎮', label: 'Gaming' },
    { icon: '📚', label: 'Books' },
    { icon: '💼', label: 'Work' },
    { icon: '🏠', label: 'Home' },
    { icon: '🎁', label: 'Gift' },
    { icon: '💾', label: 'Save' },
    { icon: '•••', label: 'More' },
  ];

  const daysInMonth = Array.from({ length: 35 }, (_, i) => i + 1);

  return (
    <div className="min-h-screen bg-gradient-to-b from-[#01143D] to-[#0335A3]">
      <StatusBar />

      <div className="px-6 pt-4 pb-8">
        <div className="flex items-center justify-between mb-6">
          <button onClick={() => navigate(-1)}>
            <ChevronLeft className="w-6 h-6 text-white" />
          </button>
          <h1 className="text-white text-[20px] font-semibold">Add Transaction</h1>
          <img src={imgProfile} alt="Profile" className="w-10 h-10 rounded-full object-cover" />
        </div>

        {/* Expenses/Income Toggle */}
        <div className="flex gap-3 mb-6">
          <button className="flex-1 bg-[#00B2FF] text-white py-3.5 rounded-[30px] font-semibold text-[18px]">
            Expenses
          </button>
          <button className="flex-1 bg-[rgba(62,78,101,0.9)] text-white py-3.5 rounded-[30px] font-semibold text-[18px]">
            Income
          </button>
        </div>

        {/* Amount and Notes */}
        <div className="space-y-4 mb-6">
          <div className="relative">
            <input
              type="number"
              placeholder="Amount"
              className="w-full bg-[rgba(62,78,101,0.8)] text-white placeholder-gray-300 px-4 py-3.5 rounded-xl outline-none focus:ring-2 focus:ring-[#00B2FF] text-[15px]"
            />
            <div className="absolute right-4 top-1/2 -translate-y-1/2 w-8 h-8 bg-[#01143D] rounded-full flex items-center justify-center">
              <span className="text-white text-[14px]">₱</span>
            </div>
          </div>

          <input
            type="text"
            placeholder="Notes"
            className="w-full bg-[rgba(62,78,101,0.8)] text-white placeholder-gray-300 px-4 py-3.5 rounded-xl outline-none focus:ring-2 focus:ring-[#00B2FF] text-[15px]"
          />
        </div>

        {/* Category */}
        <div className="mb-6">
          <h3 className="text-white text-[14px] font-medium mb-3">Category</h3>
          <div className="grid grid-cols-5 gap-3">
            {categories.map((category, index) => (
              <button
                key={index}
                className="bg-[rgba(62,78,101,0.8)] aspect-square rounded-2xl flex flex-col items-center justify-center hover:bg-[rgba(62,78,101,1)] transition-colors"
              >
                <span className="text-2xl mb-1">{category.icon}</span>
              </button>
            ))}
          </div>
        </div>

        {/* Date Picker */}
        <div className="mb-6">
          <h3 className="text-white text-[14px] font-medium mb-3">Select Date</h3>
          <div className="bg-[rgba(62,78,101,0.8)] rounded-2xl p-4">
            <div className="flex justify-around mb-4 text-[13px]">
              <button className="text-gray-300">Added</button>
              <button className="text-[#00B2FF] font-semibold">Recurring</button>
              <button className="text-gray-300">Not Available</button>
            </div>

            <div className="grid grid-cols-7 gap-2 mb-4">
              {['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'].map((day) => (
                <div key={day} className="text-center text-gray-300 text-[11px] font-medium">
                  {day}
                </div>
              ))}
            </div>

            <div className="grid grid-cols-7 gap-2">
              {daysInMonth.slice(0, 35).map((day) => (
                <button
                  key={day}
                  className={`aspect-square rounded-full flex items-center justify-center text-[13px] ${
                    day === 10
                      ? 'bg-[#00B2FF] text-white font-semibold'
                      : day > 31
                      ? 'text-gray-600'
                      : 'text-gray-200 hover:bg-[#01143D]'
                  }`}
                >
                  {day <= 31 ? day : day - 31}
                </button>
              ))}
            </div>
          </div>
        </div>

        {/* Save Button */}
        <button className="w-full bg-[#00B2FF] text-white py-4 rounded-full font-semibold hover:bg-[#43B3EF] transition-colors text-[16px]">
          Save Transaction
        </button>
      </div>
    </div>
  );
}