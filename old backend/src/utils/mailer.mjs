/**
 * @file mailer.mjs
 * @description Email sending utility using OVH SMTP (nodemailer)
 */

import nodemailer from 'nodemailer';
import { config } from '../config/env.mjs';

// Check if email credentials are configured
const isEmailConfigured = () => {
  return config.email.user && config.email.pass;
};

// Create OVH SMTP transporter with timeout settings
const createTransporter = () => {
  if (!isEmailConfigured()) {
    console.warn('⚠️ Email credentials not configured (EMAIL_USER, EMAIL_PASS)');
    return null;
  }
  
  return nodemailer.createTransport({
    host: 'ssl0.ovh.net',
    port: 465,
    secure: true,
    auth: {
      user: config.email.user,
      pass: config.email.pass
    },
    tls: {
      rejectUnauthorized: false
    },
    // Add timeout settings
    connectionTimeout: 30000, // 30 seconds
    greetingTimeout: 15000,   // 15 seconds
    socketTimeout: 30000      // 30 seconds
  });
};

let transporter = createTransporter();

/**
 * Send an email
 * @param {string} to - Recipient email
 * @param {string} subject - Email subject
 * @param {string} html - HTML content
 * @param {string} text - Plain text fallback
 */
export const sendEmail = async (to, subject, html, text = '') => {
  // Check if transporter is available
  if (!transporter) {
    const errorMsg = 'Email service not configured. Please set EMAIL_USER and EMAIL_PASS environment variables.';
    console.error('❌', errorMsg);
    return { success: false, error: errorMsg };
  }
  
  try {
    console.log('📧 Attempting to send email to:', to);
    
    const info = await transporter.sendMail({
      from: `"CCM Recrutement" <${config.email.user}>`,
      to,
      subject,
      text: text || html.replace(/<[^>]*>/g, ''),
      html
    });
    
    console.log('📧 Email sent successfully:', { to, subject, messageId: info.messageId });
    return { success: true, messageId: info.messageId };
  } catch (error) {
    // Detailed error logging
    let errorMessage = error.message;
    let errorDetails = {
      code: error.code,
      command: error.command,
      host: 'ssl0.ovh.net',
      port: 465
    };
    
    if (error.code === 'ETIMEDOUT') {
      errorMessage = `Connection timeout to SMTP server (ssl0.ovh.net:465). This may be due to firewall restrictions on the hosting platform.`;
    } else if (error.code === 'ECONNREFUSED') {
      errorMessage = `Connection refused by SMTP server. Check if the server is accessible from this environment.`;
    } else if (error.code === 'EAUTH') {
      errorMessage = `SMTP authentication failed. Check EMAIL_USER and EMAIL_PASS credentials.`;
    }
    
    console.error('❌ Error sending email:', {
      message: errorMessage,
      details: errorDetails,
      originalError: error.message
    });
    
    return { success: false, error: errorMessage, details: errorDetails };
  }
};

/**
 * Send password reset email with code
 * @param {string} to - Recipient email
 * @param {string} code - 5-digit reset code
 * @param {string} name - Recipient name
 */
export const sendPasswordResetEmail = async (to, code, name = '') => {
  const subject = 'Réinitialisation de votre mot de passe - CCM';
  
  const html = `
    <!DOCTYPE html>
    <html>
    <head>
      <meta charset="utf-8">
      <meta name="viewport" content="width=device-width, initial-scale=1.0">
    </head>
    <body style="margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f4f4;">
      <div style="max-width: 600px; margin: 0 auto; padding: 20px;">
        <div style="background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%); border-radius: 16px; overflow: hidden;">
          <!-- Header -->
          <div style="padding: 30px; text-align: center;">
            <h1 style="color: #ffffff; margin: 0; font-size: 28px;">🔐 CCM Recrutement</h1>
          </div>
          
          <!-- Content -->
          <div style="background: #ffffff; padding: 40px; border-radius: 0 0 16px 16px;">
            <h2 style="color: #1a1a2e; margin: 0 0 20px 0; font-size: 22px;">
              Bonjour${name ? ` ${name}` : ''},
            </h2>
            
            <p style="color: #555; font-size: 16px; line-height: 1.6; margin: 0 0 25px 0;">
              Vous avez demandé la réinitialisation de votre mot de passe. Utilisez le code ci-dessous pour continuer :
            </p>
            
            <!-- Code Box -->
            <div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 12px; padding: 25px; text-align: center; margin: 0 0 25px 0;">
              <p style="color: rgba(255,255,255,0.8); font-size: 14px; margin: 0 0 10px 0; text-transform: uppercase; letter-spacing: 2px;">
                Votre code de vérification
              </p>
              <div style="font-size: 36px; font-weight: bold; color: #ffffff; letter-spacing: 8px; font-family: monospace;">
                ${code}
              </div>
            </div>
            
            <p style="color: #888; font-size: 14px; line-height: 1.6; margin: 0 0 20px 0;">
              ⏰ Ce code est valable pendant <strong>15 minutes</strong>.
            </p>
            
            <p style="color: #888; font-size: 14px; line-height: 1.6; margin: 0;">
              Si vous n'avez pas demandé cette réinitialisation, vous pouvez ignorer cet email en toute sécurité.
            </p>
          </div>
        </div>
        
        <!-- Footer -->
        <div style="text-align: center; padding: 20px;">
          <p style="color: #999; font-size: 12px; margin: 0;">
            © ${new Date().getFullYear()} CCM Recrutement. Tous droits réservés.
          </p>
        </div>
      </div>
    </body>
    </html>
  `;
  
  return sendEmail(to, subject, html);
};

/**
 * Verify transporter connection
 */
export const verifyMailerConnection = async () => {
  if (!transporter) {
    console.warn('⚠️ Mail transporter not configured - email sending will be disabled');
    return false;
  }
  
  try {
    await transporter.verify();
    console.log('✅ Mail server connection verified');
    return true;
  } catch (error) {
    console.warn('⚠️ Mail server connection failed:', error.message);
    console.warn('⚠️ Note: Some hosting platforms (like Render) block outbound SMTP. Consider using a transactional email service (SendGrid, Resend, Mailgun).');
    return false;
  }
};

/**
 * Get email service status
 */
export const getEmailServiceStatus = () => {
  return {
    configured: isEmailConfigured(),
    transporterReady: !!transporter
  };
};