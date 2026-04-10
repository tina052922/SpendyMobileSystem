import { Link } from 'react-router';
import StatusBar from '../components/StatusBar';
import BottomNav from '../components/BottomNav';
import imgProfile from 'figma:asset/69ca7ab92d67aa29944f86cbdd255cc6ff75a5f7.png';

export default function SavingsScreenCustom() {
  const savingPlans = [
    { id: 1, name: 'Gaming PC', current: 30000, target: 40000, date: 'Dec 31, 2025' },
    { id: 2, name: 'Gaming PC', current: 30000, target: 40000, date: 'Dec 31, 2025' },
    { id: 3, name: 'Gaming PC', current: 30000, target: 40000, date: 'Dec 31, 2025' },
    { id: 4, name: 'Gaming PC', current: 30000, target: 40000, date: 'Dec 31, 2025' },
    { id: 5, name: 'Gaming PC', current: 30000, target: 40000, date: 'Dec 31, 2025' },
  ];

  return (
    <div className="min-h-screen bg-[#E7EBEE]">
      <div className="bg-gradient-to-b from-[#01143d] to-[#0335a3] pb-8">
        <StatusBar />
        <div className="px-6 pt-4 flex items-center justify-between">
          <div className="flex items-center gap-3">
            <img src={imgProfile} alt="Profile" className="w-10 h-10 rounded-full" />
            <h1 className="text-white text-2xl font-semibold">Savings</h1>
          </div>
          <span className="text-[#FF3B5C] text-sm font-medium">Ended</span>
        </div>
      </div>

      <div className="px-6 -mt-4 pb-28">
        <div className="space-y-4">
          {savingPlans.map((plan) => {
            const percentage = (plan.current / plan.target) * 100;
            return (
              <div
                key={plan.id}
                className="bg-white rounded-[20px] p-5 shadow-md"
              >
                <div className="flex items-start justify-between mb-2">
                  <h3 className="text-[#01143d] text-[15px] font-bold">{plan.name}</h3>
                  <Link
                    to={`/savings/${plan.id}/edit`}
                    className="text-[#022268] text-sm font-medium hover:underline"
                  >
                    Edit
                  </Link>
                </div>

                <p className="text-[#01143d] text-base font-medium mb-3">
                  ₱{plan.current.toLocaleString()} / ₱{plan.target.toLocaleString()}
                </p>

                <div className="mb-2">
                  <div className="w-full bg-[#D9D9D9] rounded-full h-1">
                    <div
                      className="bg-[#43B3EF] h-1 rounded-full transition-all"
                      style={{ width: `${percentage}%` }}
                    />
                  </div>
                </div>

                <div className="flex items-center justify-between text-xs text-[rgba(1,20,61,0.6)]">
                  <span>Target Date: {plan.date}</span>
                  <span>{percentage.toFixed(0)}% Complete</span>
                </div>
              </div>
            );
          })}
        </div>
      </div>

      <Link
        to="/savings/new/edit"
        className="fixed bottom-28 right-6 w-[50px] h-[50px] bg-white rounded-full flex items-center justify-center shadow-lg hover:shadow-xl transition-shadow"
      >
        <div className="w-10 h-10 bg-[#295DC7] rounded-full flex items-center justify-center">
          <svg className="w-5 h-5 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={3} d="M12 4v16m8-8H4" />
          </svg>
        </div>
      </Link>

      <BottomNav />
    </div>
  );
}
