import { Box, Typography, Stack, TextField, Button, Container, Link } from '@mui/material';
import { GitHub, LinkedIn, YouTube, Article } from '@mui/icons-material';

const Footer = () => {
  return (
    <Box
      sx={{
        background: 'linear-gradient(to bottom, #0b0c2a, #1a1a40)',
        color: '#fff',
        py: 8,
        mt: 10,
      }}
    >
      <Container maxWidth="md" sx={{ textAlign: 'center' }}>
        <Typography variant="h4" fontWeight="bold" gutterBottom>
          Sosyal Medya
        </Typography>
        <Typography variant="body1" sx={{ mb: 4 }}>
          Tüm içeriklerime aşağıdaki platformlardan ulaşabilirsiniz.
        </Typography>

        <Stack direction="row" justifyContent="center" spacing={4} sx={{ mb: 6 }}>
          <Link href="https://www.linkedin.com/in/fatihsultansaruhan" target="_blank" color="inherit">
            <LinkedIn fontSize="large" />
          </Link>
          <Link href="https://github.com/fsaruhan" target="_blank" color="inherit">
            <GitHub fontSize="large" />
          </Link>
          <Link href="https://www.youtube.com/@fsaruhan" target="_blank" color="inherit">
            <YouTube fontSize="large" />
          </Link>
          <Link href="https://medium.com/@fsaruhan" target="_blank" color="inherit">
            <Article fontSize="large" />
          </Link>
        </Stack>

        <Typography variant="h5" fontWeight="bold" gutterBottom>
          Yazılım Dünyasında Güncel Kal
        </Typography>
        <Typography variant="body2" sx={{ mb: 2 }}>
          .NET, Microservice ve daha fazlası için mail listeme katıl.
        </Typography>

        <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2} justifyContent="center" sx={{ mb: 6 }}>
          <TextField
            variant="outlined"
            placeholder="E-posta adresinizi girin"
            size="small"
            sx={{
              bgcolor: '#fff',
              borderRadius: 1,
              minWidth: '300px',
            }}
          />
          <Button
            variant="contained"
            color="warning"
            sx={{
              fontWeight: 'bold',
              px: 4,
            }}
          >
            Katıl
          </Button>
        </Stack>

        <Typography variant="body2" color="gray">
          © {new Date().getFullYear()} Fatih Sultan Saruhan – Tüm Hakları Saklıdır
        </Typography>
      </Container>
    </Box>
  );
};

export default Footer;
