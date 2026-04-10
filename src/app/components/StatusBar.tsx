import { Wifi, Battery } from 'lucide-react';

export default function StatusBar() {
  return (
    <div className="flex items-center justify-between px-6 py-3 text-white">
      <p className="text-sm font-medium">9:41</p>
      <div className="flex items-center gap-1">
        <Wifi className="w-4 h-4" />
        <p className="text-sm font-medium">100%</p>
        <Battery className="w-6 h-4" />
      </div>
    </div>
  );
}
