import { useState } from "react";
import { Box, Button, TextField, Typography, Alert } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { login as apiLogin } from "../../services/api";

export default function Login() {
  const navigate = useNavigate();
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);
    try {
      const res = await apiLogin(userName, password);
      // API LoginUserViewModel: Token büyük harf gelebilir
      const token = res.token || res.Token;
      if (!token) throw new Error("Sunucudan token alınamadı");
      localStorage.setItem("auth_token", token);
      localStorage.setItem("auth_user", JSON.stringify(res));
      navigate("/admin");
    } catch (err) {
      setError(err.message || "Giriş başarısız");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box sx={{ maxWidth: 400, mx: "auto", mt: 12, p: 3, boxShadow: 3, borderRadius: 2 }}>
      <Typography variant="h5" mb={2} fontWeight={700}>
        Giriş Yap
      </Typography>
      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}
      <form onSubmit={handleSubmit}>
        <TextField
          label="Kullanıcı Adı"
          value={userName}
          onChange={(e) => setUserName(e.target.value)}
          fullWidth
          margin="normal"
          autoFocus
          required
        />
        <TextField
          label="Şifre"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          fullWidth
          margin="normal"
          required
        />
        <Button
          type="submit"
          variant="contained"
          color="primary"
          fullWidth
          disabled={loading}
          sx={{ mt: 2 }}
        >
          {loading ? "Giriş yapılıyor..." : "Giriş Yap"}
        </Button>
      </form>
    </Box>
  );
}
