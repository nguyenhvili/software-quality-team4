import { Routes, Route } from "react-router-dom";
import { AppContextWrapper } from "./AppContextWrapper";
import EmailsPage from "./pages/EmailsPage";
import ReportsPage from "./pages/ReportsPage";

function App() {
  return (
    <AppContextWrapper>
      <Routes>
        <Route path="" element={<EmailsPage />} />
        <Route path="/emails" element={<EmailsPage />} />
        <Route path="/reports" element={<ReportsPage />} />
      </Routes>
    </AppContextWrapper>
  );
}

export default App;
