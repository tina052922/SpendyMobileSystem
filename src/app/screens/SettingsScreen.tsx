import { ChevronRight, Eye, LogOut, Bell } from 'lucide-react';
import { useState } from 'react';
import { Link, useNavigate } from 'react-router';
import StatusBar from '../components/StatusBar';
import BottomNav from '../components/BottomNav';

export default function SettingsScreen() {
  const [expandPassword, setExpandPassword] = useState(false);
  const navigate = useNavigate();

  const handleLogout = () => {
    navigate('/signin');
  };

  return (
    <div className="min-h-screen bg-[#E7EBEE] pb-24">
      {/* Header Section */}
      <div className="bg-gradient-to-b from-[#01143D] to-[#0335A3] pb-6">
        <StatusBar />

        <div className="px-6 pt-4">
          <div className="flex items-center justify-between">
            <h1 className="text-white text-[24px] font-bold">Settings</h1>
            <Link to="/notifications" className="w-10 h-10 rounded-full flex items-center justify-center">
              <Bell className="w-6 h-6 text-[#E7EBEE]" />
            </Link>
          </div>
        </div>
      </div>

      {/* Settings Options */}
      <div className="px-6 pt-6 space-y-4">
        {/* Profile */}
        <Link
          to="/profile"
          className="bg-white rounded-2xl p-4 flex items-center justify-between hover:bg-gray-50 transition-colors shadow-sm"
        >
          <span className="text-[#01143D] font-semibold text-[15px]">Profile</span>
          <ChevronRight className="w-5 h-5 text-gray-400" />
        </Link>

        {/* Language */}
        <div className="bg-white rounded-2xl p-4 shadow-sm">
          <div className="flex items-center justify-between">
            <span className="text-[#01143D] font-semibold text-[15px]">Language</span>
            <div className="flex items-center gap-2">
              <span className="text-[#666] text-[14px]">English</span>
              <ChevronRight className="w-5 h-5 text-gray-400" />
            </div>
          </div>
        </div>

        {/* Currency */}
        <div className="bg-white rounded-2xl p-4 shadow-sm">
          <div className="flex items-center justify-between">
            <span className="text-[#01143D] font-semibold text-[15px]">Currency</span>
            <div className="flex items-center gap-2">
              <span className="text-[#666] text-[14px]">PHP</span>
              <ChevronRight className="w-5 h-5 text-gray-400" />
            </div>
          </div>
        </div>

        {/* Update Password */}
        <div className="bg-white rounded-2xl overflow-hidden shadow-sm">
          <button
            onClick={() => setExpandPassword(!expandPassword)}
            className="w-full p-4 flex items-center justify-between hover:bg-gray-50 transition-colors"
          >
            <span className="text-[#01143D] font-semibold text-[15px]">Update Password</span>
            <ChevronRight
              className={`w-5 h-5 text-gray-400 transition-transform ${
                expandPassword ? 'rotate-90' : ''
              }`}
            />
          </button>

          {expandPassword && (
            <div className="px-4 pb-4 space-y-3 border-t border-gray-200 pt-4">
              <div className="relative">
                <input
                  type="password"
                  placeholder="Current Password"
                  className="w-full px-4 py-3 bg-[#F5F5F5] rounded-xl outline-none focus:ring-2 focus:ring-[#00B2FF] text-[14px]"
                />
                <Eye className="absolute right-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
              </div>

              <div className="relative">
                <input
                  type="password"
                  placeholder="New Password"
                  className="w-full px-4 py-3 bg-[#F5F5F5] rounded-xl outline-none focus:ring-2 focus:ring-[#00B2FF] text-[14px]"
                />
                <Eye className="absolute right-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
              </div>

              <div className="relative">
                <input
                  type="password"
                  placeholder="Confirm Password"
                  className="w-full px-4 py-3 bg-[#F5F5F5] rounded-xl outline-none focus:ring-2 focus:ring-[#00B2FF] text-[14px]"
                />
                <Eye className="absolute right-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
              </div>

              <button className="w-full bg-[#00B2FF] text-white py-3 rounded-xl font-semibold hover:bg-[#43B3EF] transition-colors text-[15px]">
                Update Password
              </button>
            </div>
          )}
        </div>

        {/* Log Out */}
        <button
          onClick={handleLogout}
          className="w-full bg-[#FF3B5C] rounded-2xl p-4 flex items-center justify-center gap-2 hover:bg-[#E6324F] transition-colors shadow-sm"
        >
          <LogOut className="w-5 h-5 text-white" />
          <span className="text-white font-semibold text-[15px]">Log out</span>
        </button>
      </div>

      <BottomNav />
    </div>
  );
}