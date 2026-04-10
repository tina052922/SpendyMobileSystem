import { Plus, Bell } from 'lucide-react';
import { Link } from 'react-router';
import StatusBar from '../components/StatusBar';
import BottomNav from '../components/BottomNav';
import imgProfile from 'figma:asset/69ca7ab92d67aa29944f86cbdd255cc6ff75a5f7.png';

export default function DashboardScreen() {
  const transactions = [
    { id: 1, category: 'Food', amount: -80, time: '12:00', icon: '🍔' },
    { id: 2, category: 'Traffic', amount: -80, time: '13:00', icon: '🚗' },
    { id: 3, category: 'Shopping', amount: -1530, time: '13:50', icon: '🛍️' },
    { id: 4, category: 'Grocery', amount: -3250, time: '14:00', icon: '🛒' },
  ];

  return (
    <div className="min-h-screen bg-[#E7EBEE]">
      {/* Header with gradient background */}
      <div className="bg-gradient-to-b from-[#01143D] to-[#0335A3] pb-12 rounded-b-[35px]">
        <StatusBar />

        <div className="px-6 pt-4">
          <div className="flex items-center justify-between mb-8">
            <img src={imgProfile} alt="Profile" className="w-10 h-10 rounded-full object-cover" />
            <Link to="/notifications" className="w-10 h-10 rounded-full flex items-center justify-center">
              <Bell className="w-6 h-6 text-[#E7EBEE]" />
            </Link>
          </div>

          {/* Balance Section */}
          <div className="text-center mb-6">
            <p className="text-[rgba(225,241,254,0.6)] text-[15px] mb-2">Available balance</p>
            <p className="text-white text-[36px] font-semibold tracking-tight">₱10,867.50</p>
          </div>

          {/* Expenses/Income Toggle */}
          <div className="flex gap-3 mb-6">
            <button className="flex-1 bg-[#022268] text-white py-3.5 rounded-[30px] font-semibold text-[20px]">
              Expenses
            </button>
            <button className="flex-1 bg-[rgba(62,78,101,0.9)] text-white py-3.5 rounded-[30px] font-semibold text-[20px]">
              Income
            </button>
          </div>
        </div>
      </div>

      {/* Total Expenditure Card */}
      <div className="px-6 -mt-6 mb-6">
        <div className="bg-white rounded-[20px] p-4 shadow-md flex items-center justify-between">
          <span className="text-[#01143D] text-[14px]">Total Expenditure</span>
          <span className="text-[#FF0000] text-[14px] font-normal">₱5,020</span>
        </div>
      </div>

      {/* Date Bar */}
      <div className="px-6 mb-4">
        <div className="bg-[#01143D] rounded-[20px] px-4 py-2 flex items-center justify-between">
          <span className="text-white font-semibold text-[16px]">Sat, 13 September</span>
          <button className="w-[30px] h-[30px] bg-[#01143D] border border-white rounded-full flex items-center justify-center">
            <svg className="w-[18px] h-[18px] text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
            </svg>
          </button>
        </div>
      </div>

      {/* Transactions List */}
      <div className="px-6 pb-24 space-y-3">
        {transactions.map((transaction) => (
          <div key={transaction.id} className="bg-white rounded-[13px] p-3 shadow-sm flex items-center justify-between">
            <div className="flex items-center gap-3">
              <div className="w-[30px] h-[30px] bg-[#01143D] rounded-[10px] flex items-center justify-center text-lg">
                {transaction.icon}
              </div>
              <span className="text-[#01143D] font-medium text-[14px]">{transaction.category}</span>
            </div>
            <div className="text-right">
              <p className="text-[#FF0000] text-[14px] font-normal">-₱{Math.abs(transaction.amount)}</p>
              <p className="text-[#949292] text-[14px]">{transaction.time}</p>
            </div>
          </div>
        ))}
      </div>

      {/* Floating Add Button */}
      <Link
        to="/add-transaction"
        className="fixed bottom-24 right-6 w-14 h-14 bg-[#00B2FF] rounded-full flex items-center justify-center shadow-lg hover:bg-[#43B3EF] transition-colors"
      >
        <Plus className="w-6 h-6 text-white" />
      </Link>

      <BottomNav />
    </div>
  );
}