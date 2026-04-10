import { Calendar, Bell } from 'lucide-react';
import { Link } from 'react-router';
import { BarChart, Bar, XAxis, YAxis, ResponsiveContainer } from 'recharts';
import StatusBar from '../components/StatusBar';
import BottomNav from '../components/BottomNav';
import imgProfile from 'figma:asset/69ca7ab92d67aa29944f86cbdd255cc6ff75a5f7.png';

export default function StatisticsScreen() {
  const chartData = [
    { id: 1, day: '1', amount: 100 },
    { id: 2, day: '2', amount: 140 },
    { id: 3, day: '3', amount: 130 },
    { id: 4, day: '4', amount: 90 },
    { id: 5, day: '5', amount: 170 },
    { id: 6, day: '6', amount: 190 },
    { id: 7, day: '7', amount: 230 },
    { id: 8, day: '8', amount: 140 },
    { id: 9, day: '9', amount: 160 },
    { id: 10, day: '10', amount: 120 },
    { id: 11, day: '11', amount: 90 },
  ];

  const categories = [
    { id: 1, name: 'Food', amount: 2560, icon: '🍔' },
    { id: 2, name: 'Traffic', amount: 820, icon: '🚗' },
    { id: 3, name: 'Shopping', amount: 2560, icon: '🛍️' },
  ];

  return (
    <div className="min-h-screen bg-[#E7EBEE] pb-24">
      {/* Header Section */}
      <div className="bg-gradient-to-b from-[#01143D] to-[#0335A3] pb-6">
        <StatusBar />

        <div className="px-6 pt-4">
          <div className="flex items-center justify-between mb-6">
            <img src={imgProfile} alt="Profile" className="w-10 h-10 rounded-full object-cover" />
            <Link to="/notifications" className="w-10 h-10 rounded-full flex items-center justify-center">
              <Bell className="w-6 h-6 text-[#E7EBEE]" />
            </Link>
          </div>

          {/* Balance Card */}
          <div className="bg-gradient-to-r from-[rgba(27,43,66,0.8)] to-[rgba(10,20,40,0.8)] rounded-2xl p-4 mb-6">
            <p className="text-[rgba(239,234,234,0.7)] text-[15px] mb-1">Available balance</p>
            <p className="text-white text-[36px] font-medium tracking-tight">₱10,867.50</p>
          </div>

          {/* Expenses/Income Toggle */}
          <div className="flex gap-3">
            <button className="flex-1 bg-[#022268] text-white py-3.5 rounded-[30px] font-semibold text-[20px]">
              Expenses
            </button>
            <button className="flex-1 bg-[rgba(62,78,101,0.9)] text-white py-3.5 rounded-[30px] font-semibold text-[20px]">
              Income
            </button>
          </div>
        </div>
      </div>

      {/* Chart Section */}
      <div className="px-6 pt-6">
        <div className="flex items-center justify-between mb-4">
          <p className="text-[#01143D] font-semibold text-[15px]">September 2025</p>
          <button className="w-10 h-10 bg-[#01143D] rounded-full flex items-center justify-center">
            <Calendar className="w-6 h-6 text-white" />
          </button>
        </div>

        {/* Bar Chart */}
        <div className="bg-white rounded-2xl p-4 mb-6 shadow-sm">
          <ResponsiveContainer width="100%" height={180}>
            <BarChart data={chartData}>
              <XAxis 
                dataKey="day" 
                stroke="transparent" 
                tick={{ fill: '#01143D', fontSize: 12 }} 
                axisLine={false}
              />
              <YAxis 
                stroke="transparent" 
                tick={{ fill: '#01143D', fontSize: 12 }} 
                axisLine={false}
              />
              <Bar 
                dataKey="amount" 
                fill="url(#barGradient)" 
                radius={[4, 4, 0, 0]} 
                key="amount-bar"
              />
              <defs>
                <linearGradient id="barGradient" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="0%" stopColor="#022268" />
                  <stop offset="100%" stopColor="rgba(239,234,234,0.6)" />
                </linearGradient>
              </defs>
            </BarChart>
          </ResponsiveContainer>
        </div>

        {/* Categories Section */}
        <h3 className="text-[#01143D] text-[15px] font-semibold mb-4">September 2025</h3>
        <div className="space-y-3">
          {categories.map((category) => (
            <div key={category.id} className="bg-white rounded-[13px] p-4 shadow-sm flex items-center justify-between">
              <div className="flex items-center gap-3">
                <div className="w-[30px] h-[30px] bg-[#01143D] rounded-[10px] flex items-center justify-center text-lg">
                  {category.icon}
                </div>
                <p className="font-medium text-[#01143D] text-[14px]">{category.name}</p>
              </div>
              <p className="font-normal text-[#01143D] text-[14px]">₱{category.amount}</p>
            </div>
          ))}
        </div>
      </div>

      <BottomNav />
    </div>
  );
}