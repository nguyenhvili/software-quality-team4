import { Routes, Route } from "react-router-dom";
import { AppContextWrapper } from "./AppContextWrapper";
import EmailsPage from "./pages/EmailsPage";
import ReportsPage from "./pages/ReportsPage";
import { Toaster } from "react-hot-toast";

function App() {
  return (
    <AppContextWrapper>
      <Routes>
        <Route path="" element={<EmailsPage />} />
        <Route path="/emails" element={<EmailsPage />} />
        <Route path="/reports" element={<ReportsPage />} />
      </Routes>
      <Toaster position="top-right" />
    </AppContextWrapper>
  );
}

export default App;
