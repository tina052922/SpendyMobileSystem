import { Link } from 'react-router';
import StatusBar from '../components/StatusBar';
import BottomNav from '../components/BottomNav';
import imgProfile from 'figma:asset/69ca7ab92d67aa29944f86cbdd255cc6ff75a5f7.png';

export default function SavingsScreen() {
  const savingPlans = [
    { id: 1, name: 'Gaming PC', current: 30000, target: 40000, date: 'Dec 31, 2025' },
    { id: 2, name: 'Vacation Fund', current: 25000, target: 50000, date: 'Jun 15, 2026' },
    { id: 3, name: 'Emergency Fund', current: 35000, target: 40000, date: 'Dec 31, 2025' },
  ];

  return (
    <div className="min-h-screen bg-[#E7EBEE]">
      {/* Header */}
      <div className="bg-gradient-to-b from-[#01143D] to-[#0335A3] pb-8">
        <StatusBar />
        <div className="px-6 pt-4 flex items-center justify-between">
          <div className="flex items-center gap-3">
            <img src={imgProfile} alt="Profile" className="w-10 h-10 rounded-full object-cover" />
            <h1 className="text-white text-[24px] font-bold">Savings</h1>
          </div>
          <span className="text-[#FF3B5C] text-[14px] font-semibold">Ended</span>
        </div>
      </div>

      {/* Savings Plans */}
      <div className="px-6 -mt-4 pb-28">
        <div className="space-y-4">
          {savingPlans.map((plan) => {
            const percentage = (plan.current / plan.target) * 100;
            return (
              <div
                key={plan.id}
                className="bg-white rounded-[20px] p-5 shadow-sm"
              >
                <div className="flex items-start justify-between mb-3">
                  <Link to={`/savings/${plan.id}`} className="flex-1">
                    <h3 className="text-[#01143D] text-[16px] font-bold">{plan.name}</h3>
                  </Link>
                  <Link
                    to={`/savings/${plan.id}/edit`}
                    className="text-[#00B2FF] text-[14px] font-semibold hover:underline"
                    onClick={(e) => e.stopPropagation()}
                  >
                    Edit
                  </Link>
                </div>

                <Link to={`/savings/${plan.id}`}>
                  <p className="text-[#01143D] text-[15px] font-medium mb-3">
                    ₱{plan.current.toLocaleString()} / ₱{plan.target.toLocaleString()}
                  </p>

                  {/* Progress Bar */}
                  <div className="mb-3">
                    <div className="w-full bg-[#D9D9D9] rounded-full h-2">
                      <div
                        className="bg-[#00B2FF] h-2 rounded-full transition-all"
                        style={{ width: `${percentage}%` }}
                      />
                    </div>
                  </div>

                  <div className="flex items-center justify-between text-[12px] text-[rgba(1,20,61,0.6)]">
                    <span>Target Date: {plan.date}</span>
                    <span className="font-medium">{percentage.toFixed(0)}% Complete</span>
                  </div>
                </Link>
              </div>
            );
          })}
        </div>
      </div>

      {/* Floating Add Button */}
      <Link
        to="/savings/new/edit"
        className="fixed bottom-28 right-6 w-14 h-14 bg-[#00B2FF] rounded-full flex items-center justify-center shadow-lg hover:bg-[#43B3EF] transition-colors"
      >
        <svg className="w-6 h-6 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={3} d="M12 4v16m8-8H4" />
        </svg>
      </Link>

      <BottomNav />
    </div>
  );
}